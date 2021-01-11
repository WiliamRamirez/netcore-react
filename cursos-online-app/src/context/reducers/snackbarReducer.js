import { CLEAN_SNACKBAR, OPEN_SNACKBAR } from "../../types";

export const initialState = {
  mensaje: null,
  open: false,
};

const snackbarReducer = (state = initialState, action) => {
  switch (action.type) {
    case OPEN_SNACKBAR:
      return {
        mensaje: action.payload.mensaje,
        open: action.payload.open,
      };
    case CLEAN_SNACKBAR:
      return {
        mensaje: "",
        open: false,
      };
    default:
      return state;
  }
};

export default snackbarReducer;
