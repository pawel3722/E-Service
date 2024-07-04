import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function NewPart() {
  const [partSerialNum, setPartSerialNum] = useState()
  const [partModelId, setPartModelId] = useState()
  const [models, setModels] = useState([])

  const navigate = useNavigate();
  const location = useLocation();

  const axiosPrivate = useAxiosPrivate();

  async function createPart() {
    var serialNumber = partSerialNum ? partSerialNum : ''
    var modelId = partModelId ? Number(partModelId) : -1

    if (modelId !== -1 && partSerialNum !== '') {
      try {
        // make axios post request
        await axiosPrivate.post('/api/Part',
          JSON.stringify({ serialNumber, modelId }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        )
        alert("Dodano część!")
      } catch (error) {
        console.log(error)
      }
    }
    else
      alert("Proszę wprowadzić poprawne dane!")

  }

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
      <div>Dodaj część</div>
      {
        <form onSubmit={() => createPart()}>
          <label>Model:</label><br></br>
          <select name="modelId" onChange={(e) => setPartModelId(e.target.value)}>
            <option value={-1}>Wybierz model</option>
            {
              models.map((m) => <option value={m.id}>{m.name}, {m.type}</option>)
            }
          </select><br></br>
          <label>Numer seryjny:</label><br></br>
          <input type="text" name="serialNum" onChange={(e) => setPartSerialNum(e.target.value)} /><br></br>
          <input type="submit" value="Zapisz"></input>
        </form>
      }

    </>
  )
}

export default NewPart