import { AppBar } from "@material-ui/core";
import React from "react";
import { useStateValue } from "../../context/Store";
import BarSesion from "./Bar/BarSesion";

const AppNavbar = () => {
  const [{ auth }] = useStateValue();

  return auth ? (
    auth.autenticado ? (
      <AppBar position="static">
        <BarSesion />
      </AppBar>
    ) : null
  ) : null;
};

export default AppNavbar;
