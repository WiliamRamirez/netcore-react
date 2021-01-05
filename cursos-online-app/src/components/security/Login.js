import {
  Avatar,
  Button,
  Container,
  TextField,
  Typography,
} from "@material-ui/core";
import React, { useState } from "react";
import style from "../tools/Style";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import { loginUsuario } from "../../actions/UsuarioAction";

const Login = () => {
  const [usuario, setUsuario] = useState({
    email: "",
    password: "",
  });

  const handleChange = (event) => {
    const { value, name } = event.target;
    setUsuario((before) => ({
      ...before,
      [name]: value,
    }));
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    loginUsuario(usuario).then((response) => {
      console.log("login exitoso", response);
      window.localStorage.setItem("token_seguridad", response.data.token);
    });
  };

  return (
    <Container maxWidth="xs">
      <div style={style.paper}>
        <Avatar style={style.avatar}>
          <LockOutlinedIcon style={style.icon} />
        </Avatar>
        <Typography component="h1" variant="h5">
          Login Usuario
        </Typography>

        <form style={style.form}>
          <TextField
            variant="outlined"
            label="Ingrese Email"
            name="email"
            value={usuario.email}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />
          <TextField
            variant="outlined"
            type="password"
            label="Ingrese password"
            name="password"
            value={usuario.password}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />

          <Button
            type="submit"
            fullWidth
            onClick={handleSubmit}
            variant="contained"
            color="primary"
            style={style.submit}
          >
            Enviar
          </Button>
        </form>
      </div>
    </Container>
  );
};

export default Login;
