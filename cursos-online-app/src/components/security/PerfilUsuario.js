import React, { Fragment, useEffect, useState } from "react";
import style from "../tools/Style";
import {
  Avatar,
  Button,
  Container,
  Grid,
  TextField,
  Typography,
} from "@material-ui/core";
import { actualizarUsuario } from "../../actions/UsuarioAction";
import imagenPerfilUsuario from "../../logo.svg";
import { v4 as uuidv4 } from "uuid";
import ImageUploader from "react-images-upload";
import { obtenerDataImagen } from "../../actions/ImagenAction";
import { useStateValue } from "../../context/Store";
import { OPEN_SNACKBAR } from "../../types";

const PerfilUsuario = () => {
  const [{ auth }, dispatch] = useStateValue();
  const { usuario } = auth;

  const [user, setUsuario] = useState({
    nombreCompleto: "",
    email: "",
    username: "",
    password: "",
    confirmPassword: "",
    imagenPerfilUrl: "",
    imagenPerfil: null,
  });

  const { nombreCompleto, email, username, password, confirmPassword } = user;

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUsuario({
      ...user,
      [name]: value,
    });
  };

  useEffect(() => {
    setUsuario(usuario);
    setUsuario((before) => ({
      ...before,
      imagenPerfilUrl: usuario.imagenPerfil,
    }));
  }, [usuario]);

  const handleSubmit = (event) => {
    event.preventDefault();
    actualizarUsuario(user, dispatch).then((response) => {
      console.log("log de actualizaUsuario", response);

      if (response.status === 200) {
        dispatch({
          type: OPEN_SNACKBAR,
          payload: {
            mensaje: "Los cambios se guardaron exitosamente",
            open: true,
          },
        });
        window.localStorage.setItem("token_seguridad", response.data.token);
      } else {
        dispatch({
          type: OPEN_SNACKBAR,
          payload: {
            mensaje:
              "Los siguientes campos son obligatorios: " +
              Object.keys(response.data.errors),
            open: true,
          },
        });
      }
    });
  };

  const handleimagenPerfil = (imagenes) => {
    const imagenPerfil = imagenes[0];
    const imagenPerfilUrl = URL.createObjectURL(imagenPerfil);

    obtenerDataImagen(imagenPerfil).then((respuesta) => {
      console.log(respuesta);
      setUsuario((before) => ({
        ...before,
        imagenPerfil: respuesta,
        imagenPerfilUrl: imagenPerfilUrl,
      }));
    });
  };

  const imagenPerfilKey = uuidv4();

  return (
    <Fragment>
      <Container component="main" maxWidth="md" justify="center">
        <div style={style.paper}>
          <Avatar
            style={style.avatar}
            src={user.imagenPerfilUrl || imagenPerfilUsuario}
          />
          <Typography component="h1" variant="h5">
            Perfil del Usuario
          </Typography>
          <form style={style.form}>
            <Grid container spacing={2}>
              <Grid item xs={12} md={12}>
                <TextField
                  name="nombreCompleto"
                  onChange={handleChange}
                  value={nombreCompleto || ""}
                  variant="outlined"
                  fullWidth
                  label="Ingrese Nombre y Apellido"
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  name="email"
                  onChange={handleChange}
                  value={email || ""}
                  variant="outlined"
                  fullWidth
                  label="Ingrese email"
                />
              </Grid>

              <Grid item xs={12} md={6}>
                <TextField
                  name="username"
                  onChange={handleChange}
                  value={username || ""}
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
                  value={password || ""}
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
                  value={confirmPassword || ""}
                  variant="outlined"
                  fullWidth
                  label="Confirme password"
                />
              </Grid>
              <Grid item xs={12} md={12}>
                <ImageUploader
                  withIcon={false}
                  key={imagenPerfilKey}
                  singleImage={true}
                  buttonText="Seleccione una imagen de perfil"
                  onChange={handleimagenPerfil}
                  imgExtension={[".jpg", ".gif", ".png", ".jpeg"]}
                  maxFileSize={5242880}
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
        </div>
      </Container>
    </Fragment>
  );
};

export default PerfilUsuario;
