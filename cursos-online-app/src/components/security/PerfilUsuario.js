import React, { Fragment, useContext, useEffect, useState } from "react";
import style from "../tools/Style";
import {
  Button,
  Container,
  Grid,
  TextField,
  Typography,
} from "@material-ui/core";
import {
  actualizarUsuario,
  obtenerUsuarioActual,
} from "../../actions/UsuarioAction";
import SnackbarContext from "../../context/snackbar/SnackbarContext";
import SnackbarValidation from "../helpers/SnackbarValidation";

const PerfilUsuario = () => {

  const snackbarContext = useContext(SnackbarContext);
  const { openSnackbar } = snackbarContext;

  const [usuario, setUsuario] = useState({
    nombreCompleto: "",
    email: "",
    username: "",
    password: "",
    confirmPassword: "",
  });

  const {
    nombreCompleto,
    email,
    username,
    password,
    confirmPassword,
  } = usuario;

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUsuario({
      ...usuario,
      [name]: value,
    });
  };

  useEffect(() => {
    obtenerUsuarioActual().then((response) => {
      setUsuario(response.data);
    });
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault();
    actualizarUsuario(usuario).then((response) => {
      console.log("log de actualizaUsuario", response);
      if (response.status === 200) {
        openSnackbar("Los cambios se guardaron exitosamente");
        window.localStorage.setItem("token_seguridad", response.data.token);
      } else {
        openSnackbar("Los siguientes campos son obligatorios: " + Object.keys(response.data.errors));
      }
    });
  };

  return (
    <Fragment>
      <SnackbarValidation />
      <Container component="main" maxWidth="md" justify="center">
        <div style={style.paper}>
          <Typography component="h1" variant="h5">
            Perfil del Usuario
          </Typography>
        </div>

        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={12}>
              <TextField
                name="nombreCompleto"
                onChange={handleChange}
                value={nombreCompleto}
                variant="outlined"
                fullWidth
                label="Ingrese Nombre y Apellido"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="email"
                onChange={handleChange}
                value={email}
                variant="outlined"
                fullWidth
                label="Ingrese email"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="username"
                onChange={handleChange}
                value={username}
                variant="outlined"
                fullWidth
                label="Ingrese username"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="password"
                type="password"
                onChange={handleChange}
                value={password}
                variant="outlined"
                fullWidth
                label="Ingrese password"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="confirmPassword"
                type="password"
                onChange={handleChange}
                value={confirmPassword}
                variant="outlined"
                fullWidth
                label="Confirme password"
              />
            </Grid>
          </Grid>
          <Grid container justify="center">
            <Grid item xs={12} md={6}>
              <Button
                type="submit"
                fullWidth
                variant="contained"
                size="large"
                color="primary"
                style={style.submit}
                onClick={handleSubmit}
              >
                Guardar Datos
              </Button>
            </Grid>
          </Grid>
        </form>
      </Container>
    </Fragment>
  );
};

export default PerfilUsuario;
