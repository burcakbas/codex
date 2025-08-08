import EmployeeForm from '../../../components/EmployeeForm';

export default async function EditEmployee({ params }: { params: { id: string } }) {
  const res = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/employees/${params.id}`, { cache: 'no-store' });
  if (!res.ok) {
    return <div>Not found</div>;
  }
  const employee = await res.json();
  return (
    <div>
      <h1 className="text-xl mb-4">Edit Employee</h1>
      <EmployeeForm employee={employee} />
    </div>
  );
}
