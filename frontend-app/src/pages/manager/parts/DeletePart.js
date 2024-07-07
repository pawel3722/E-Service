import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation, useParams } from "react-router-dom";

function DeletePart() {
  const [modelName, setModelName] = useState()
  const [partSerialNum, setPartSerialNum] = useState()
  const [modelType, setModelType] = useState()
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams()

  const axiosPrivate = useAxiosPrivate();

  async function deletePart() {
    try {
      // make axios post request
      await axiosPrivate.delete('/api/Part/' + id)
      alert("Usunięto część!")
    } catch (error) {
      console.log(error)
      alert(error)
    }
  }

  useEffect(() => {
    const getPart = async () => {
      try {
        const response = await axiosPrivate.get('/api/Part/' + id)
        console.log(response.data)    
        setPartSerialNum(response.data.serialNumber)
        const response2 = await axiosPrivate.get('/api/Model/' + response.data.modelId)
        setModelName(response2.data.name)
        setModelType(response2.data.type)

      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
    getPart()
  }, [])

  return (
    <>
      <div>Czy na pewno chcesz usunąć tę część?</div>
      {
        <form onSubmit={() => deletePart()}>
          <label>Nazwa: {modelName}</label><br></br>          
          <label>Typ: {modelType}</label><br></br>
          <label>Numer seryjny: {partSerialNum}</label><br></br>
          <input type="submit" value="Usuń"></input>
        </form>
      }

    </>
  )
}

export default DeletePart