import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation, useParams } from "react-router-dom";

function EditModel() {
  const [modelName, setModelName] = useState()
  const [modelType, setModelType] = useState()
  const [modelPrice, setModelPrice] = useState()
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams()

  const axiosPrivate = useAxiosPrivate();

  async function editModel() {
    var name = modelName ? modelName : ''
    var type = modelType ? modelType : ''
    var price = modelPrice ? Number(modelPrice) : 0

    if (name !== '' && type !== '') {
      try {
        // make axios post request
        await axiosPrivate.put('/api/Model/' + id,
          JSON.stringify({ name, price, type }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        )
        alert("Dodano model!")
      } catch (error) {
        console.log(error)
      }
    }
    else
      alert("Proszę wprowadzić poprawne dane!")

  }

  useEffect(() => {
    const getServiceType = async () => {
      try {
        const response = await axiosPrivate.get('/api/Model/' + id)
        console.log(response.data)
        setModelName(response.data.name)
        setModelType(response.data.type)
        setModelPrice(response.data.price)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
    getServiceType()
  }, [])

  return (
    <>
      <div>Edytuj model</div>
      {
        <form onSubmit={() => editModel()}>
          <label>Nazwa:</label><br></br>
          <input type="text" name="name" defaultValue={modelName} onChange={(e) => setModelName(e.target.value)} /><br></br>
          <label>Typ:</label><br></br>
          <input type="text" name="device" defaultValue={modelType} onChange={(e) => setModelType(e.target.value)} /><br></br>
          <label>Cena:</label><br></br>
          <input type="number" name="minPrice" defaultValue={modelPrice} min="0" onChange={(e) => setModelPrice(e.target.value)} /><br></br>
          <input type="submit" value="Zapisz"></input>
        </form>
      }

    </>
  )
}

export default EditModel