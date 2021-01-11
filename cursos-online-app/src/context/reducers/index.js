import authReducer from "./authReducer";
import snackbarReducer from "./snackbarReducer";

export const mainReducer = ({ auth, snackbar }, action) => {
  return {
    auth: authReducer(auth, action),
    snackbar: snackbarReducer(snackbar, action),
  };
};
