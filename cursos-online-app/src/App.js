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
import RutaSegura from "./components/Navigation/RutaSegura";
import NuevoCurso from "./components/cursos/NuevoCurso";
import PaginadorCurso from "./components/cursos/PaginadorCurso";

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

      <Router>
        <MuiThemeProvider theme={theme}>
          <AppNavbar />
          <Grid container>
            <Switch>
              <Route exact path="/auth/login" component={Login} />
              <Route
                exact
                path="/auth/registrar"
                component={RegistrarUsuario}
              />

              <RutaSegura exact path="/auth/perfil" component={PerfilUsuario} />
              <RutaSegura exact path="/curso/nuevo" component={NuevoCurso} />

              <RutaSegura
                exact
                path="/curso/paginador"
                component={PaginadorCurso}
              />
            </Switch>
          </Grid>
        </MuiThemeProvider>
      </Router>
    </Fragment>
  );
}

export default App;
