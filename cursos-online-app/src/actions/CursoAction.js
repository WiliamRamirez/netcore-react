import HttpCliente from "../services/HttpCliente";

export const guardarCurso = async (curso, imagen) => {
  const endPointCurso = "/Cursos";
  const promesaCurso = HttpCliente.post(endPointCurso, curso);

  if (imagen) {
    const endPointImagen = "/Documento";
    const promesaImagen = HttpCliente.post(endPointImagen, imagen);
    return await Promise.all([promesaCurso, promesaImagen]);
  } else {
    return await Promise.all([promesaCurso]);
  }
};

export const paginacionCurso = (paginador) => {
  return new Promise((resolve, eject) => {
    HttpCliente.post("/cursos/report", paginador).then(response => {
      resolve(response);
    });
  })
}
