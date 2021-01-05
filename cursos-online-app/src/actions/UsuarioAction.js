import HttpCliente from "../services/HttpCliente";

export const registrarUsuario = (user) => {
  return new Promise((resolve, eject) => {
    HttpCliente.post("/usuario/registrar", user).then((response) => {
      resolve(response);
    });
  });
};

export const obtenerUsuarioActual = () => {
  return new Promise((resolve, eject) => {
    HttpCliente.get("/usuario").then((response) => {
      resolve(response);
    });
  });
};

export const actualizarUsuario = (user) => {
  return new Promise((resolve, eject) => {
    HttpCliente.put("/usuario/", user)
      .then((response) => {
        resolve(response);
      })
      .catch((error) => {
        resolve(error.response);
      });
  });
};

export const loginUsuario = (user) => {
  return new Promise((resolve, eject) => {
    HttpCliente.post("/usuario/login", user).then((response) => {
      resolve(response);
    });
  });
};
