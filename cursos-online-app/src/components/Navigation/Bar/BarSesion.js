import {
  Avatar,
  Button,
  Drawer,
  IconButton,
  makeStyles,
  Toolbar,
  Typography,
} from "@material-ui/core";
import React, { Fragment, useContext, useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import AuthContext from "../../../context/auth/authContext";
import FotoUsuario from "../../../logo.svg";
import MenuLeft from "./MenuLeft";
import MenuRight from "./MenuRight";

const useStyles = makeStyles((theme) => ({
  seccionDesktop: {
    display: "none",
    [theme.breakpoints.up("md")]: {
      display: "flex",
    },
  },
  seccionMovil: {
    display: "flex",
    [theme.breakpoints.up("md")]: {
      display: "none",
    },
  },
  grow: {
    flexGrow: 1,
  },
  avatarSize: {
    width: 40,
    height: 40,
  },
  list: {
    width: 250,
  },
  listItemText: {
    fontSize: "14ps",
    fontWeight: 600,
    paddingLeft: "15px",
    color: "#212121",
  },
}));

const BarSesion = (props) => {
  const classes = useStyles();

  const authContext = useContext(AuthContext);
  const { usuario, usuarioAutenticado } = authContext;

  const [stateMenuLeft, setStateMenuLeft] = useState(false);

  const [stateMenuRight, setStateMenuRight] = useState(false);

  const closeMenuLeft = () => {
    setStateMenuLeft(false);
  };

  const openMenuLeft = () => {
    setStateMenuLeft(true);
  };

  const closeMenuRight = () => {
    setStateMenuRight(false);
  };

  const openMenuRight = () => {
    setStateMenuRight(true);
  };

  const closeSessionApp = () => {
    localStorage.removeItem("token_seguridad");
    props.history.push("/auth/login");
  };

  useEffect(() => {
    usuarioAutenticado();
    // eslint-disable-next-line
  }, []);

  return (
    <Fragment>
      <Drawer open={stateMenuLeft} onClose={closeMenuLeft} anchor="left">
        <div
          className={classes.list}
          onKeyDown={closeMenuLeft}
          onClick={closeMenuLeft}
        >
          <MenuLeft classes={classes} />
        </div>
      </Drawer>

      <Drawer open={stateMenuRight} onClose={closeMenuRight} anchor="right">
        <div
          className={classes.list}
          onKeyDown={closeMenuRight}
          onClick={closeMenuRight}
        >
          <MenuRight
            classes={classes}
            salirSesion={closeSessionApp}
            usuario={usuario}
          />
        </div>
      </Drawer>

      <Toolbar>
        <IconButton color="inherit" onClick={openMenuLeft}>
          <i className="material-icons">menu</i>
        </IconButton>
        <Typography variant="h6"> Cursos Online </Typography>

        <div className={classes.grow}></div>

        {usuario ? (
          <div className={classes.seccionDesktop}>
            <Avatar src={FotoUsuario}></Avatar>
            <Button color="inherit"> {usuario.nombreCompleto} </Button>
            <Button color="inherit" onClick={closeSessionApp}>
              Salir
            </Button>
          </div>
        ) : null}

        <div className={classes.seccionMovil}>
          <IconButton color="inherit" onClick={openMenuRight}>
            <i className="material-icons">more_vert</i>
          </IconButton>
        </div>
      </Toolbar>
    </Fragment>
  );
};

export default withRouter(BarSesion);
