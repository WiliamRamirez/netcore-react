import axios from "axios";
import HttpCliente from "../services/HttpCliente";
import { ACTUALIZAR_USUARIO, INICIAR_SESION } from "../types";

const instancia = axios.create();
instancia.CancelToken = axios.CancelToken;
instancia.isCancel = axios.isCancel;

export const registrarUsuario = (user) => {
  return new Promise((resolve, eject) => {
    instancia.post("/usuario/registrar", user).then((response) => {
      resolve(response);
    });
  });
};

export const obtenerUsuarioActual = (dispatch) => {
  return new Promise((resolve, eject) => {
    HttpCliente.get("/usuario")
      .then((response) => {
        if (response.data.imagenPerfil && response.data) {
          let fotoPerfil = response.data.imagenPerfil;
          const newFile =
            "data:image/" + fotoPerfil.extension + ";base64," + fotoPerfil.data;
          response.data.imagenPerfil = newFile;
        }
        dispatch({
          type: INICIAR_SESION,
          payload: {
            sesion: response.data,
            autenticado: true,
          },
        });
        resolve(response);
      })
      .catch((error) => {
        resolve(error.response);
      });
  });
};

export const actualizarUsuario = (user, dispatch) => {
  return new Promise((resolve, eject) => {
    HttpCliente.put("/usuario/", user)
      .then((response) => {
        resolve(response);
        if (response.data.imagenPerfil && response.data) {
          let fotoPerfil = response.data.imagenPerfil;
          const newFile =
            "data:image/" + fotoPerfil.extension + ";base64," + fotoPerfil.data;
          response.data.imagenPerfil = newFile;
        }
        dispatch({
          type: ACTUALIZAR_USUARIO,
          payload: {
            nuevaSesion: response.data,
            autenticado: true,
          },
        });
      })
      .catch((error) => {
        resolve(error.response);
      });
  });
};

export const loginUsuario = (user, dispatch) => {
  return new Promise((resolve, eject) => {
    instancia
      .post("/usuario/login", user)
      .then((response) => {
        if (response.data.imagenPerfil && response.data) {
          let fotoPerfil = response.data.imagenPerfil;
          const newFile =
            "data:image/" + fotoPerfil.extension + ";base64," + fotoPerfil.data;
          response.data.imagenPerfil = newFile;
        }
        dispatch({
          type: INICIAR_SESION,
          payload: {
            sesion: response.data,
            autenticado: true,
          },
        });
        resolve(response);
      })
      .catch((error) => {
        resolve(error.response);
      });
  });
};
