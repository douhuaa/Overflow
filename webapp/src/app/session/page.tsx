import ErrorButtons from "@/app/session/errorButtons";
import AuthTestButton from "@/app/session/authTestButton";


export default function Page() {
	return (
		<div className="px-6">
			<div className='text-center'>
				<h3 className='text-3xl'>Session dashboard</h3>
			</div>

			<div className='flex items-center gap-3 justify-center mt-6'>
				<ErrorButtons/>
				<AuthTestButton/>
			</div>
		</div>
	);
}
