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
import ClientHomePageWidget from './components/ClientHomePageWidget';
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
import ManageService from './pages/serviceman/ManageService';
import Models from './pages/manager/models/Models';
import NewModel from './pages/manager/models/NewModel';
import EditModel from './pages/manager/models/EditModel';
import DeleteModel from './pages/manager/models/DeleteModel';
import Parts from './pages/manager/parts/Parts';
import NewPart from './pages/manager/parts/NewPart';
import DeletePart from './pages/manager/parts/DeletePart';
import ManagerHomePageWidget from './components/ManagerHomePageWidget';
import ServicemanHomePageWidget from './components/ServicemanHomePageWidget';
import SellerHomePageWidget from './components/SellerHomePageWidget';
import Register from './pages/Register';


function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="home" element={<Home />} />
        <Route path="uslugi" element={<Uslugi />} />
        <Route path="kontakt" element={<Kontakt />} />
        <Route path="log" element={<Log />} />
        <Route path="register" element={<Register />} />

        <Route element={<PersistLogin />}>
          <Route element={<ProtectedRoutes />} >
            <Route path='/user' element={<UserPage />} >
              <Route path='home' element={<ClientHomePageWidget />} />
              <Route path='client-orders' element={<ClientOrders />} />
              <Route path='new-order' element={<NewOrder />} />
              <Route path='deliver-order' element={<DeliverOrder />} />
              <Route path='pay-for-order' element={<PayForOrder />} />
              <Route path='assign-manager-to-order' element={<AssignManagerToOrder />} />
              <Route path='service-types' element={<ServiceTypes />} />
              <Route path='new-service-type' element={<NewServiceType />} />
              <Route path='edit-service-type/:id' element={<EditServiceType />} />            
              <Route path='delete-service-type/:id' element={<DeleteServiceType />} />
              <Route path='models' element={<Models />} />
              <Route path='new-model' element={<NewModel />} />
              <Route path='edit-model/:id' element={<EditModel />} />
              <Route path='delete-model/:id' element={<DeleteModel />} />
              <Route path='parts' element={<Parts />} />
              <Route path='new-part' element={<NewPart />} />
              <Route path='delete-part/:id' element={<DeletePart />} />
              <Route path='assign-worker-to-service' element={<AssignWorkerToService />} />
              <Route path='assign-services-to-order' element={<AssignServicesToOrder />} />
              <Route path='new-service/:id' element={<NewService />} />
              <Route path='serviceman-services' element={<ServicemanServices />} />
              <Route path='manage-service/:id' element={<ManageService />} />
              <Route path='manager' element={<ManagerHomePageWidget />} />
              <Route path='serviceman' element={<ServicemanHomePageWidget />} />
              <Route path='seller' element={<SellerHomePageWidget />} />
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
