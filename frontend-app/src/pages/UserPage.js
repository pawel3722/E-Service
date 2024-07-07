import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useNavigate, useLocation, Outlet } from "react-router-dom";
import NavbarUser from '../components/NavbarUser';
import UserSideBar from '../components/UserSideBar';
import './UserPage.css'

function UserPage() {
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {

    const path = location.pathname

    const getUsers = async () => {
      try {
        const response = await axiosPrivate.get('/api/Auth/users/me')
        console.log(response.data)
        response.data.roles.find((r) => r.name == "Client")
          ? navigate('/user/home')
          : navigate("/home")
          // switch (key) {
          //   case value:
              
          //     break;
          
          //   default:
          //     break;
          // }

      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getUsers()

  }, [])

  return (
    <>
      <NavbarUser />
      <UserSideBar />
      <div className='layout'>
        <Outlet />
      </div>
    </>
  )
}

export default UserPage