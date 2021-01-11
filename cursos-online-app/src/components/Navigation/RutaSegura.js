import React from "react";
import { Redirect, Route } from "react-router-dom";
import { useStateValue } from "../../context/Store";

function RutaSegura({ component: Component, ...rest }) {
  const [{ auth }] = useStateValue();

  return (
    <Route
      {...rest}
      render={(props) =>
        auth ? (
          auth.autenticado === true ? (
            <Component {...props} {...rest} />
          ) : (
            <Redirect to="/auth/login" />
          )
        ) : (
          <Redirect to="/auth/login" />
        )
      }
    />
  );
}

export default RutaSegura;
