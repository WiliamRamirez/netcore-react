import { ThemeProvider as MuiThemeProvider } from "@material-ui/core/styles";
import theme from "./theme/theme";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import Login from "./components/security/Login";
import PerfilUsuario from "./components/security/PerfilUsuario";
import RegistrarUsuario from "./components/security/RegistrarUsuario";
import { Grid, Snackbar } from "@material-ui/core";
import AppNavbar from "./components/Navigation/AppNavbar";
import { Fragment, useEffect, useState } from "react";
import { obtenerUsuarioActual } from "./actions/UsuarioAction";
import { useStateValue } from "./context/Store";
import { CLEAN_SNACKBAR } from "./types";

function App() {
  
  const [{ snackbar }, dispatch] = useStateValue();

  const [startApp, setStartApp] = useState(false);

  useEffect(() => {
    if (!startApp) {
      obtenerUsuarioActual(dispatch)
        .then((response) => {
          setStartApp(true);
        })
        .catch((error) => {
          setStartApp(true);
        });
    }
    // eslint-disable-next-line
  }, [startApp]);

  return startApp === false ? null : (
    <Fragment>
      <Router>
        <MuiThemeProvider theme={theme}>
          <Snackbar
            anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
            open={snackbar ? snackbar.open : false}
            autoHideDuration={3000}
            ContentProps={{ "aria-describedby": "message-id" }}
            message={
              <span id="message-id">{snackbar ? snackbar.mensaje : ""} </span>
            }
            onClose={() =>
              dispatch({
                type: CLEAN_SNACKBAR,
              })
            }
          ></Snackbar>

          <AppNavbar />
          <Grid container>
            <Switch>
              <Route exact path="/auth/login" component={Login} />
              <Route
                exact
                path="/auth/registrar"
                component={RegistrarUsuario}
              />
              <Route exact path="/auth/perfil" component={PerfilUsuario} />
            </Switch>
          </Grid>
        </MuiThemeProvider>
      </Router>
    </Fragment>
  );
}

export default App;
