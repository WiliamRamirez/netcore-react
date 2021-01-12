import React, { useState } from "react";
import {
  Button,
  Container,
  Grid,
  InputAdornment,
  TextField,
  Typography,
} from "@material-ui/core";
import style from "../tools/Style";
import {
  KeyboardDatePicker,
  MuiPickersUtilsProvider,
} from "@material-ui/pickers";
import DateFnsUtils from "@date-io/date-fns";
import ImageUploader from "react-images-upload";
import { v4 as uuidv4 } from "uuid";
import { obtenerDataImagen } from "../../actions/ImagenAction";
import { guardarCurso } from "../../actions/CursoAction";
import { useStateValue } from "../../context/Store";
import { openSnackbar } from "../../actions/SnackbarAction";

const NuevoCurso = () => {
  const [, dispatch] = useStateValue();

  const [curso, setCurso] = useState({
    titulo: "",
    descripcion: "",
    precio: 0.0,
    promocion: 0.0,
  });

  const { titulo, descripcion, precio, promocion } = curso;

  const [error, setError] = useState({
    tituloError: "",
    descripcionError: "",
    precioError: "",
    promocionError: "",
  });

 
  const [imagenCurso, setImagenCurso] = useState(null);

  const [dateTime, setDateTime] = useState(new Date());

  const validate = () => {
    let temp = {};
    temp.tituloError = titulo ? "" : "Este campo es Requerido";
    temp.descripcionError = descripcion ? "" : "Este campo es Requerido";
    temp.precioError = parseFloat(precio) > 0 ? "" : "Este campo es requerido";
    temp.promocionError =
      parseFloat(promocion) > 0 ? "" : "Este campo es requerido";

    setError({
      ...error,
      ...temp,
    });

    return Object.values(temp).every((x) => x === "");
  };

  // Pasando datos al state de la memoria
  const handleChange = (event) => {
    const { name, value } = event.target;

    setCurso({
      ...curso,
      [name]: value,
    });
  };

  // Limpiar los valoroes del form y de los errores
  const restarForm = () => {
    setError({
      tituloError: "",
      descripcionError: "",
      precioError: "",
      promocionError: "",
    });

    setCurso({
      titulo: "",
      descripcion: "",
      precio: 0.0,
      promocion: 0.0,
    });
    setDateTime(new Date());
    setImagenCurso(null);

  };

  const handleChangeFoto = (imagenes) => {
    const foto = imagenes[0];
    obtenerDataImagen(foto).then((respuesta) => {
      setImagenCurso(respuesta);
    });
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    const isError = validate();

    if (!isError) {
      return;
    }

    const cursoId = uuidv4();
    const objetoCurso = {
      titulo: curso.titulo,
      descripcion: curso.descripcion,
      promocion: parseFloat(curso.promocion ? curso.promocion : 0.0),
      precio: parseFloat(curso.precio ? curso.precio : 0.0),
      fechaPublicacion: dateTime,
      cursoId: cursoId,
    };

    let objetoImagen = null;

    if (imagenCurso) {
      objetoImagen = {
        nombre: imagenCurso.nombre,
        data: imagenCurso.data,
        extension: imagenCurso.extension,
        objetoReferencia: cursoId,
      };
    }

    guardarCurso(objetoCurso, objetoImagen).then((respuestas) => {
      const responseCurso = respuestas[0];
      const responseImagen = respuestas[1];
      let mensaje = "";

      if (responseCurso.status === 200) {
        mensaje += "Se guardo exitosamente el curso";
      } else {
        mensaje +=
          "Errores en Curso: " + Object.keys(responseCurso.data.errors);
      }

      if (responseImagen) {
        if (responseImagen.status === 200) {
          mensaje += "Se guardo exitosamente la Imagen";
        } else {
          mensaje +=
            ", Errores en Imagen: " + Object.keys(responseImagen.data.errors);
        }
      }

      restarForm();
      openSnackbar(dispatch, mensaje);
    });
  };

  const fotoKey = uuidv4();

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography component="h1" variant="h5">
          Registro de Nuevo Curso
        </Typography>

        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={12}>
              <TextField
                name="titulo"
                value={titulo}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                label="Ingrese Titulo"
                error={!!error.tituloError}
                helperText={error.tituloError}
              />
            </Grid>
            <Grid item xs={12} md={12}>
              <TextField
                name="descripcion"
                value={descripcion}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                label="Ingrese descripcion"
                error={!!error.descripcionError}
                helperText={error.descripcionError}
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="precio"
                value={precio}
                type="number"
                onChange={handleChange}
                variant="outlined"
                fullWidth
                label="Ingrese Precio Normal"
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">S/.</InputAdornment>
                  ),
                }}
                error={!!error.precioError}
                helperText={error.precioError}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="promocion"
                value={promocion}
                type="number"
                onChange={handleChange}
                variant="outlined"
                fullWidth
                label="Ingrese Precio Promoción"
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">S/.</InputAdornment>
                  ),
                }}
                error={!!error.promocionError}
                helperText={error.promocionError}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <MuiPickersUtilsProvider utils={DateFnsUtils}>
                <KeyboardDatePicker
                  value={dateTime}
                  onChange={setDateTime}
                  margin="normal"
                  id="fecha-publicacion-id"
                  label="Seleccione Fecha de Publicación"
                  format="dd/MM/yyyy"
                  fullWidth
                  KeyboardButtonProps={{
                    "aria-label": "change date",
                  }}
                />
              </MuiPickersUtilsProvider>
            </Grid>

            <Grid item xs={12} md={6}>
              <ImageUploader
                withIcon={false}
                key={fotoKey}
                singleImage={true}
                buttonText="Seleccione Imagen del Curso"
                onChange={handleChangeFoto}
                imgExtension={[".jpg", ".gif", ".png", ".jpeg"]}
                maxFileSize={5242880}
              />
            </Grid>
          </Grid>
          <Grid container justify="center">
            <Grid item xs={12} md={12}>
              <Button
                type="submit"
                fullWidth
                variant="contained"
                color="primary"
                size="large"
                style={style.submit}
                onClick={handleSubmit}
              >
                Guardar Curso
              </Button>
            </Grid>
          </Grid>
        </form>
      </div>
    </Container>
  );
};

export default NuevoCurso;
