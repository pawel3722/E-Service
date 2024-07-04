import { Route, Routes } from 'react-router-dom';
import './App.css';
import Uslugi from './pages/Uslugi';
import Kontakt from './pages/Kontakt';
import Log from './pages/Log';
import Home from './pages/Home';
import UserPage from './pages/UserPage';
import ClientOrders from './pages/client/ClientOrders';
import ProtectedRoutes from './context/ProtectedRoutes';
import PersistLogin from './components/PersistLogin';
import HomePageWidget from './components/HomePageWidget';
import NewOrder from './pages/seller/NewOrder'
import DeliverOrder from './pages/seller/DeliverOrder'
import PayForOrder from './pages/seller/PayForOrder';
import AssignManagerToOrder from './pages/manager/AssignManagerToOrder';
import ServiceTypes from './pages/manager/service-types/ServiceTypes';
import NewServiceType from './pages/manager/service-types/NewServiceType'
import EditServiceType from './pages/manager/service-types/EditServiceType'
import DeleteServiceType from './pages/manager/service-types/DeleteServiceType'
import AssignServicesToOrder from './pages/manager/AssignServicesToOrder';
import AssignWorkerToService from './pages/manager/AssignWorkerToService'
import NewService from './pages/manager/NewService';
import ServicemanServices from './pages/serviceman/ServicemanServices';


function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="home" element={<Home />} />
        <Route path="uslugi" element={<Uslugi />} />
        <Route path="kontakt" element={<Kontakt />} />
        <Route path="log" element={<Log />} />

        <Route element={<PersistLogin />}>
          <Route element={<ProtectedRoutes />} >
            <Route path='user' element={<UserPage />} >
              <Route path='home' element={<HomePageWidget />} />
              <Route path='client-orders' element={<ClientOrders />} />
              <Route path='new-order' element={<NewOrder />} />
              <Route path='deliver-order' element={<DeliverOrder />} />
              <Route path='pay-for-order' element={<PayForOrder />} />
              <Route path='assign-manager-to-order' element={<AssignManagerToOrder />} />
              <Route path='service-types' element={<ServiceTypes />} />
              <Route path='new-service-type' element={<NewServiceType />} />
              <Route path='edit-service-type/:id' element={<EditServiceType />} />
              <Route path='delete-service-type/:id' element={<DeleteServiceType />} />
              <Route path='assign-worker-to-service' element={<AssignWorkerToService />} />
              <Route path='assign-services-to-order' element={<AssignServicesToOrder />} />
              <Route path='new-service/:id' element={<NewService />} />
              <Route path='serviceman-services' element={<ServicemanServices />} />
            </Route>
          </Route>
        </Route>

        {/* <Route element={<PersistLogin />}>
          <Route element={<ProtectedRoutes />} >
          <Route path='client-orders' element={<ClientOrders />} />
          </Route>
        </Route> */}
      </Routes>
    </div>
  );
}

export default App;
