import React, { useEffect, useState } from "react";


export default function ControlTyping(texto, delay) {
    const [textoValor, setTextoValor] = useState() 
    
    useEffect(() => {
      const manejador = setTimeout(() => {
        setTextoValor(texto);
      }, delay);

      return () => {
        clearTimeout(manejador);
      };
    }, [texto, delay]);

    return textoValor
}