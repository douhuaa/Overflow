import {Question} from "@/lib/types";
import {Chip} from "@heroui/chip";
import Link from "next/link";
import {Avatar} from "@heroui/avatar";
import {formatIsoToLocal} from "@/lib/date";

type Props = {
	quesiton: Question
}
export default function QuestionFooter({quesiton}: Props) {
	return (
		<div className='flex justify-between mt-2'>
			<div className='flex flex-col self-end'>
				<div className='flex gap-2'>
					{quesiton.tagSlugs.map(tag => (
						<Link href={`/questions?tag=${tag}`} key={tag}>
							<Chip variant='bordered'>
								{tag}
							</Chip>
						</Link>
					))}
				</div>
			</div>
			<div className='flex flex-col basis-2/5  bg-primary/10 px-3 gap-2 rounded-lg'>
				<span className='text-sm font-extralight'>
					asked {formatIsoToLocal(quesiton.createdAt)}
				</span>
				<div className='flex items-center gap-3'>
					<Avatar className='h-6 w-6'
							color='secondary'
							name={quesiton.askerDisplayName.charAt(0)}/>
					<div className='flex flex-col items-center'>
						<span>{quesiton.askerDisplayName}</span>
						<span className='self-start text-sm font-semibold'>42</span>
					</div>
				</div>
			</div>
		</div>
	);
}
