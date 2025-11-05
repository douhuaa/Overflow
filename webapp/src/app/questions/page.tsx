import {getQuestions} from "@/lib/actions/question-actions";
import QuestionCard from "@/app/questions/question-card";
import QuestionHeader from "@/app/questions/QuestionHeader";

export default async function QuestionsPage({
												searchParams,
											}: {
	searchParams: Promise<{ tag?: string | string[] }>;
}) {
	const sp = await searchParams;
	const tagParam = sp?.tag;
	const tag = Array.isArray(tagParam) ? tagParam[0] : tagParam;
	const questions = await getQuestions(tag);
	return (
		<>
			<QuestionHeader total={questions.length} tag={tag}/>
			{questions.map((question) => (
				<div key={question.id} className='py-4 not-last:border-b w-full flex'>
					<QuestionCard key={question.id} question={question}/>
				</div>
			))}
		</>
	);
}
