import {
  Avatar,
  Divider,
  List,
  ListItem,
  ListItemText,
} from "@material-ui/core";
import React from "react";
import FotoUsuario from "../../../logo.svg";
import { Link } from "react-router-dom";

const MenuRight = ({ classes, usuario, salirSesion }) => {
  return (
    <div className={classes.list}>
      <List>
        <ListItem button component={Link}>
          <Avatar src={usuario.imagenPerfil || FotoUsuario} />
          <ListItemText
            classes={{ primary: classes.listItemText }}
            primary={usuario ? usuario.nombreCompleto : ""}
          />
        </ListItem>
      </List>

      <Divider />

      <List>
        <ListItem button component={Link}>
          <i className="material-icons"> exit_to_app</i>
          <ListItemText
            classes={{ primary: classes.listItemText }}
            primary="Salir"
          />
        </ListItem>
      </List>
    </div>
  );
};

export default MenuRight;
