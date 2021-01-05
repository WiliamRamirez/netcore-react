import { CLEAN_SNACKBAR, OPEN_SNACKBAR } from "../../types";

// eslint-disable-next-line import/no-anonymous-default-export
export default (state, action) => {
  switch (action.type) {
    case OPEN_SNACKBAR: {
      return {
        ...state,
        open: true,
        mensaje: action.payload,
      };
    }
    case CLEAN_SNACKBAR:
      return {
        ...state,
        open: false,
        mensaje: "",
      };
    default:
      return state;
  }
};
