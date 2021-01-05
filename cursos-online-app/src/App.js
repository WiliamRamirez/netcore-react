import { ThemeProvider as MuiThemeProvider } from "@material-ui/core/styles";
import theme from "./theme/theme";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import Login from "./components/security/Login";
import PerfilUsuario from "./components/security/PerfilUsuario";
import RegistrarUsuario from "./components/security/RegistrarUsuario";
import { Grid } from "@material-ui/core";
import AppNavbar from "./components/Navigation/AppNavbar";
import AuthState from "./context/auth/authState";
import SnackbarState from "./context/snackbar/SnackbarState";

function App() {
  
  return (
    <SnackbarState>
      <AuthState>
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
                <Route exact path="/auth/perfil" component={PerfilUsuario} />
              </Switch>
            </Grid>
          </MuiThemeProvider>
        </Router>
      </AuthState>
    </SnackbarState>
  );
}

export default App;
