import {
  Button,
  Container,
  Grid,
  TextField,
  Typography,
} from "@material-ui/core";
import React, { useState } from "react";
import style from "../tools/Style";
import { registrarUsuario } from "../../actions/UsuarioAction";


export const RegistrarUsuario = () => {

  const [usuario, setUsuario] = useState({
    NombreCompleto: "",
    Email: "",
    Password: "",
    ConfirmPassword: "",
    Username: "",
  });

  const ingresarValoresMemoria = e => {
    const { name, value } = e.target;
    setUsuario((before) => ({
      ...before,
      [name]: value,
    }));
  }

  const registrarUsuarioBtn = (e) => {
    e.preventDefault();
    registrarUsuario(usuario).then(response => {
      console.log("Se registro existosamente el usuario", response);
      window.localStorage.setItem("token_seguridad", response.data.token);
      
    });
  };

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography component="h1" variant="h5">
          Registrar Usuario
        </Typography>

        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={12}>
              <TextField
                name="NombreCompleto"
                value={usuario.NombreCompleto}
                onChange={ingresarValoresMemoria}
                variant="outlined"
                fullWidth
                label="Ingrese su Nombre Completo"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="Email"
                value={usuario.Email}
                onChange={ingresarValoresMemoria}
                variant="outlined"
                fullWidth
                label="Ingrese su Email"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="Username"
                value={usuario.Username}
                onChange={ingresarValoresMemoria}
                variant="outlined"
                fullWidth
                label="Ingrese su UserName"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="Password"
                type="password"
                value={usuario.Password}
                onChange={ingresarValoresMemoria}
                variant="outlined"
                fullWidth
                label="Ingrese Password"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="ConfirmPassword"
                type="password"
                value={usuario.ConfirmPassword}
                onChange={ingresarValoresMemoria}
                variant="outlined"
                fullWidth
                label="Confirme Password"
              />
            </Grid>
          </Grid>

          <Grid container justify="center">
            <Grid item xs={12} md={6}>
              <Button
                type="submit"
                fullWidth
                onClick={registrarUsuarioBtn}
                variant="contained"
                color="primary"
                size="large"
                style={style.submit}
              >
                Enviar
              </Button>
            </Grid>
          </Grid>
        </form>
      </div>
    </Container>
  );
};

export default RegistrarUsuario;
