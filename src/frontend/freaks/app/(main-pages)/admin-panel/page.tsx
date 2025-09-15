import AdminLayout from "./components/AdminLayout";
import UsersTable from "./components/UsersTable";

const AdminPanelPage = () => {
  return (
    <AdminLayout>
      <UsersTable />
    </AdminLayout>
  );
};

export default AdminPanelPage;
