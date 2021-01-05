import React, { useReducer } from "react";
import { obtenerUsuarioActual } from "../../actions/UsuarioAction";
import { OBTENER_USUARIO } from "../../types";
import AuthContext from "./authContext";
import authReducer from "./authReducer";


const AuthState = (props) => {
  const initialState = {
    usuario: null,
    autenticado: false,
  };

  // Dispatch para ejecutar las acciones
  const [state, dispatch] = useReducer(authReducer, initialState);

  // Funciones

  // Retornar el usuario autenticado

  const usuarioAutenticado = () => {
    obtenerUsuarioActual().then(response => {
      dispatch({
        type: OBTENER_USUARIO,
        payload : response.data
      })
    })
  }

  return (
    <AuthContext.Provider
      value={{
        usuario: state.usuario,
        autenticado: state.autenticado,
        usuarioAutenticado,
      }}
    >
      {props.children}
    </AuthContext.Provider>
  );
};

export default AuthState;