import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function Parts() {
  const [parts, setParts] = useState([])
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
    const getParts = async () => {
      try {
        const response = await axiosPrivate.get('/api/Part')
        console.log(response.data)
        setParts(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getParts()
    getModels()
  }, [])

  return (
    <>

      <div>Części:</div>
      <button onClick={() => navigate('/user/new-part', { state: { from: location }, replace: false } )}>Dodaj</button>
      {
        parts
          ? (
            parts.map((p) => (
              <p>
                Model: {models.find(m => m.id === p.modelId) ? models.find(m => m.id === p.modelId).name : ""}<br></br>
                Numer seryjny: {p.serialNumber}<br></br>
                <button onClick={() => navigate('/user/delete-part/' + p.id, { state: { from: location }, replace: false })}>Usuń</button>
              </p>
            ))
          )
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default Parts