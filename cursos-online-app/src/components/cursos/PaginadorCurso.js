import {
  Grid,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  TextField,
} from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { paginacionCurso } from "../../actions/CursoAction";
import ControlTyping from "../tools/ControlTyping";

const PaginadorCurso = () => {
  const [textoBusquedaCurso, setTextoBusquedaCurso] = useState("");

  const typingBuscadorTexto = ControlTyping(textoBusquedaCurso, 900);

  const [paginadorRequest, setPaginadorRequest] = useState({
    titulo: "",
    numeroPagina: 0,
    cantidadElementos: 5,
  });

  const [paginadorResponse, setPaginadorResponse] = useState({
    listaRecords: [],
    totalRecords: 0,
    numeroPaginas: 0,
  });

  useEffect(() => {
    const obtenerListaCurso = async () => {
      let tituloVariant = "";
      let paginaVariant = paginadorRequest.numeroPagina + 1;

      if (typingBuscadorTexto) {
        tituloVariant = typingBuscadorTexto;
        paginaVariant = 1;
      }

      const objetoPaginadorRequest = {
        titulo: tituloVariant,
        numeroPagina: paginaVariant,
        cantidadElementos: paginadorRequest.cantidadElementos,
      };

      const response = await paginacionCurso(objetoPaginadorRequest);
      console.log(response);
      setPaginadorResponse(response.data);
    };

    obtenerListaCurso();
  }, [paginadorRequest, typingBuscadorTexto]);

  const cambiarPagina = (event, nuevaPagina) => {
    setPaginadorRequest({
      ...paginadorRequest,
      numeroPagina: parseInt(nuevaPagina),
    });
  };

  const cambiarCantidadRecords = (event) => {
    setPaginadorRequest({
      ...paginadorRequest,
      cantidadElementos: parseInt(event.target.value),
      numeroPagina: 0,
    });
  };

  return (
    <div style={{ padding: "10px", width: "100%" }}>
      <Grid container style={{ paddingTop: "20px", paddingBottom: "20px" }}>
        <Grid item xs={12} md={6}>
          <TextField
            fullWidth
            name="textBusquedaCurso"
            variant="outlined"
            label="Busca tu curso"
            onChange={(e) => setTextoBusquedaCurso(e.target.value)}
          />
        </Grid>
      </Grid>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell align="left">Cursos</TableCell>
              <TableCell align="left">Descripción</TableCell>
              <TableCell align="left">Fecha Publicacion</TableCell>
              <TableCell align="left">Precio Original</TableCell>
              <TableCell align="left">Precio Promocion</TableCell>
            </TableRow>
          </TableHead>

          <TableBody>
            {paginadorResponse.listaRecords.map((curso) => (
              <TableRow key={curso.CursoId}>
                <TableCell align="left">{curso.Titulo}</TableCell>
                <TableCell align="left">{curso.Descripcion}</TableCell>
                <TableCell align="left">
                  {new Date(curso.FechaPublicacion).toLocaleString()}
                </TableCell>
                <TableCell align="left">{curso.PrecioActual}</TableCell>
                <TableCell align="left">{curso.Promocion}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <TablePagination
        component="div"
        rowsPerPageOptions={[5, 10, 25]}
        count={paginadorResponse.totalRecords}
        rowsPerPage={paginadorRequest.cantidadElementos}
        page={paginadorRequest.numeroPagina}
        onChangePage={cambiarPagina}
        onChangeRowsPerPage={cambiarCantidadRecords}
        labelRowsPerPage="Curso por pagina"
      />
    </div>
  );
};

export default PaginadorCurso;
