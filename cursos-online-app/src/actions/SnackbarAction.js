import { OPEN_SNACKBAR } from "../types";

export const openSnackbar = (dispatch, mensaje) => {
  dispatch({
    type: OPEN_SNACKBAR,
    payload: {
      mensaje: mensaje,
      open: true,
    },
  });
};
