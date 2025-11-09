'use client'
import {Button} from "@heroui/button";
import {triggerError} from "@/lib/actions/error-actions";
import {useTransition} from "react";

export default function ErrorButtons() {
	const [pending, startTransition] = useTransition();
	const onClick = (code: number) => {
		startTransition(async () => {
			const {error} = await triggerError(code);
			if (error) throw new Error(error.message);
		})
	}

	return (
		<div className='flex gap-6 items-center mt-6 w-full justify-center'>
			{[400, 401, 403, 404, 500].map(code => (
				<Button
					onPress={async () => onClick(code)}
					color='primary'
					key={code}
					type='button'
					isLoading={pending}
				>
					Test {code}
				</Button>
			))}

		</div>
	);
}
