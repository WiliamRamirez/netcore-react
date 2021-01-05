import { Snackbar } from '@material-ui/core';
import React, { useContext } from 'react';
import SnackbarContext from '../../context/snackbar/SnackbarContext';

const SnackbarValidation = () => {

    const snackbarContext = useContext(SnackbarContext);

    const { open, mensaje, limpiarSnackbar } = snackbarContext;
    
    return (
      <Snackbar
        anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
        open={snackbarContext ? open : false}
        autoHideDuration={3000}
        ContentProps={{ "aria-describedby": "message-id" }}
        message={<span id="message-id">{snackbarContext ? mensaje : ""} </span>}
        onClose={limpiarSnackbar}
      ></Snackbar>
    );
};

export default SnackbarValidation;