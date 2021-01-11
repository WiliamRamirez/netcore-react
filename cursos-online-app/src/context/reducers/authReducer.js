import { ACTUALIZAR_USUARIO, INICIAR_SESION, SALIR_SESION } from "../../types";

export const initialState = {
  usuario: null,
  autenticado: false,
};

const authReducer = (state = initialState, action) => {
  switch (action.type) {
    case INICIAR_SESION:
      return {
        ...state,
        usuario: action.payload.sesion,
        autenticado: action.payload.autenticado,
      };
    case SALIR_SESION:
      return {
        ...state,
        usuario: action.payload.nuevaSesion,
        autenticado: action.payload.autenticado,
      };
    case ACTUALIZAR_USUARIO:
      return {
        ...state,
        usuario: action.payload.nuevaSesion,
        autenticado: action.payload.autenticado,
      };

    default:
      return state;
  }
};

export default authReducer;
