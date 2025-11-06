import {Question} from "@/lib/types";
import Link from "next/link";
import {Button} from "@heroui/button";
import {formatIsoToLocal} from "@/lib/date";
import {Avatar} from "@heroui/avatar";

type Props = {
	question: Question;
}

export default function QuestionDetailedHeader({question}: Props) {
	return (
		<div className='flex flex-col w-full border-b gap-4 pb-4 px-6'>
			<div className="flex justify-between gap-4">
				<div className='text-3xl font-semibold first-letter:uppercase '>
					{question.title}
				</div>
				<Link href='/questions/ask'>
					<Button color='secondary'> Ask Question</Button>
				</Link>
			</div>
			<div className="text-sm text-foreground-500 flex items-center gap-2">
				<Avatar className="h-6 w-6" color="secondary" name={question.askerDisplayName.charAt(0)}/>
				<Link href={`/profiles/${question.askerId}`}>{question.askerDisplayName}</Link>
				<span title={question.createdAt}>asked {formatIsoToLocal(question.createdAt)}</span>
				{question.updatedAt && (
					<span title={question.updatedAt}>Modified {formatIsoToLocal(question.createdAt)}</span>
				)}
				<span>·</span>
				<span>{question.viewCount} {question.viewCount === 1 ? 'view' : 'views'}</span>
			</div>
		</div>
	);
}
