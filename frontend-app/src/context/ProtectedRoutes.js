import React from 'react'
import { Outlet, Navigate, useLocation } from 'react-router-dom'
import useAuth from '../hooks/useAuth'


function ProtectedRoutes() {
    const { auth } = useAuth()
    const location = useLocation()

    return (
        auth?.logged
        ? <Outlet />
        : <Navigate to='/log' state={{ from: location }} replace />)
}

export default ProtectedRoutes