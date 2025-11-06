import {getQuestion} from "@/lib/actions/question-actions";
import Link from "next/link";
import {Chip} from "@heroui/chip";
import {notFound} from "next/navigation";
import QuestionDetailedHeader from "@/app/questions/[id]/QuestionDetailedHeader";
import QuestionContent from "@/app/questions/[id]/QuestionContent";
import AnswerContent from "@/app/questions/[id]/AnswerContent";
import AnswersHeader from "@/app/questions/[id]/AnswersHeader";

export const revalidate = 0;
export const dynamic = 'force-dynamic';
export const fetchCache = 'force-no-store';

export default async function QuestionDetailsPage({params}: { params: Promise<{ id: string }> }) {
	const {id} = await params;
	const question = await getQuestion(id);
	if (!question) return notFound();
	return (
		<div className="w-full">

			<QuestionDetailedHeader question={question}/>
			<QuestionContent question={question}/>
			{question.answers.length > 0 && (
				<AnswersHeader answerCount={question.answers.length}/>
			)}
			{question.answers.map(answer => (
				<AnswerContent answer={answer} key={answer.id}/>
			))}
		</div>
	);
}
