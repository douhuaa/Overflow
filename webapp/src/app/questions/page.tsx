import {getQuestions} from "@/lib/actions/question-actions";
import QuestionCard from "@/app/questions/question-card";
import QuestionHeader from "@/app/questions/QuestionHeader";

export default async function QuestionsPage({
												searchParams,
											}: {
	searchParams: Promise<{ tag?: string }>;
}) {
	const params = await searchParams;
	const {data: questions, error} = await getQuestions(params?.tag)
	if (error) throw error;
	return (
		<>
			<QuestionHeader total={questions?.length || 0} tag={params?.tag}/>
			{questions?.map((question) => (
				<div key={question.id} className='py-4 not-last:border-b w-full flex'>
					<QuestionCard key={question.id} question={question}/>
				</div>
			))}
		</>
	);
}
