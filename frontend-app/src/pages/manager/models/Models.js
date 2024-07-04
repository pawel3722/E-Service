import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function Models() {
  const [models, setModels] = useState([])
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const getModels = async () => {
      try {
        const response = await axiosPrivate.get('/api/Model')
        console.log(response.data)
        setModels(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
    getModels()
  }, [])

  return (
    <>

      <div>Modele:</div>
      <button onClick={() => navigate('/user/new-model', { state: { from: location }, replace: false } )}>Dodaj</button>
      {
        models
          ? (
            models.map((m) => (
              <p>
                Model: {m.name}<br></br>
                Typ: {m.type}<br></br>
                Cena: {m.price}<br></br>
                Ilość sztuk: {m.parts.length}<br></br>
                <button onClick={() => navigate('/user/edit-model/' + m.id, { state: { from: location }, replace: false } )}>Edytuj</button>
                <button onClick={() => navigate('/user/delete-model/' + m.id, { state: { from: location }, replace: false })}>Usuń</button>
              </p>
            ))
          )
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default Models