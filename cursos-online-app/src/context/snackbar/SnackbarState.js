import React, { useReducer } from "react";
import { CLEAN_SNACKBAR, OPEN_SNACKBAR } from "../../types";
import SnackbarContext from "./SnackbarContext";
import snackbarReducer from "./snackbarReducer";

const SnackbarState = (props) => {
  const initialState = {
    mensaje: "",
    open: false,
  };

  // Dispatch para ejecutar las accione
  const [state, dispatch] = useReducer(snackbarReducer, initialState);

  const openSnackbar = (msj) => {
    dispatch({
      type: OPEN_SNACKBAR,
      payload : msj
    })
  }
  // Funciones
  const limpiarSnackbar = () => {
    dispatch({
      type: CLEAN_SNACKBAR,
    });
  };

  return (
    <SnackbarContext.Provider
      value={{
        open: state.open,
        mensaje: state.mensaje,
        limpiarSnackbar,
        openSnackbar,
      }}
    >
      {props.children}
    </SnackbarContext.Provider>
  );
};

export default SnackbarState;
