import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation, useParams } from "react-router-dom";

function DeleteModel() {
  const [orderName, setOrderName] = useState()
  const [orderType, setOrderType] = useState()
  const [orderPrice, setOrderPrice] = useState()
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams()

  const axiosPrivate = useAxiosPrivate();

  async function deleteModel() {
    try {
      // make axios post request
      await axiosPrivate.delete('/api/Model/' + id)
      alert("Usunięto model!")
    } catch (error) {
      console.log(error)
    }
  }

  useEffect(() => {
    const getModel = async () => {
      try {
        const response = await axiosPrivate.get('/api/Model/' + id)
        console.log(response.data)
        setOrderName(response.data.name)
        setOrderType(response.data.deviceType)
        setOrderPrice(response.data.minPrice)

      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
    getModel()
  }, [])

  return (
    <>
      <div>Czy na pewno chcesz usunąć ten model?</div>
      {
        <form onSubmit={() => deleteModel()}>
          <label>Nazwa: {orderName}</label><br></br>
          <label>Typ: {orderType}</label><br></br>
          <label>Cena: {orderPrice}</label><br></br>
          <input type="submit" value="Usuń"></input>
        </form>
      }

    </>
  )
}

export default DeleteModel