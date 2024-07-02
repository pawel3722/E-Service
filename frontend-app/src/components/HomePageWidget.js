import React from 'react'
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useState, useEffect } from "react";
import { useNavigate, useLocation, Outlet } from "react-router-dom";
import ClientHomePageWidget from './ClientHomePageWidget';
import SellerHomePageWidget from './SellerHomePageWidget';


function HomePageWidget() {
  const [users, setUsers] = useState();
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {

    const getUsers = async () => {
      try {
        const response = await axiosPrivate.get('/api/Auth/users/me')
        console.log(response.data)
        setUsers(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getUsers()

  }, [])

  return (
    <>
      {
        users
          ?
          (
            console.log(users.roles),
            users.roles.find((r) => r.name === "Seller")
              ? <SellerHomePageWidget />
              : users.roles.find((r) => r.name === "Client")
                ? <ClientHomePageWidget />
                : <p>Strona główna w przygotowaniu.</p>
          )
          : <p>Użytkownik nie należy do żadnej roli!</p>
      }
      <Outlet />
    </>
  )
}

export default HomePageWidget