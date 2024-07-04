import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";


function NewModel() {
  const [modelName, setModelName] = useState()
  const [modelType, setModelType] = useState()
  const [modelPrice, setModelPrice] = useState()

  const axiosPrivate = useAxiosPrivate();

  async function createModel() {
    var name = modelName ? modelName : ''
    var type = modelType ? modelType : ''
    var price = modelPrice ? Number(modelPrice) : 0

    if(name !== '' && type !== '') {
        try {
          // make axios post request
          await axiosPrivate.post('/api/Model',
            JSON.stringify({ name, type, price }),
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

 

  return (
    <>
      <div>Dodaj model</div>
      {
        <form onSubmit={() => createModel()}>
        <label>Nazwa:</label><br></br>
        <input type="text" name="name" onChange={(e) => setModelName(e.target.value)}/><br></br>
        <label>Typ:</label><br></br>
        <input type="text" name="type" onChange={(e) => setModelType(e.target.value)}/><br></br>
        <label>Cena:</label><br></br>
        <input type="number" name="price" defaultValue="0" min="0" onChange={(e) => setModelPrice(e.target.value)}/><br></br>
        <input type="submit" value="Zapisz"></input>
    </form>
      }

    </>
  )
}

export default NewModel