import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useNavigate, useLocation, Outlet } from "react-router-dom";

function UserPage() {
  const [users, setUsers] = useState();
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    navigate('/user/home', { state: { from: location }, replace: true })
  }, [])

  return (
    <>
      <Outlet />
    </>
  )
}

export default UserPage