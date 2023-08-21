const feedbackTextarea = document.querySelector(".feedback-textarea");
const submitButton = document.querySelector(".submit-button");
const feedbackPrompt = document.querySelector(".feedback-prompt");

let feedbackSubmitted = false; // Flag to track if feedback has been submitted

submitButton.addEventListener("click", (event) => {
    event.preventDefault(); // Prevent the default form submission behavior
    if (!feedbackSubmitted) {
        const feedback = feedbackTextarea.value.trim(); // Trim whitespace from the input
        if (feedback !== "") { // Check if feedback is not empty
            // Perform further processing or send the feedback to the server
            console.log("Feedback submitted:", feedback);
            feedbackTextarea.value = feedback; // Display the feedback in the textarea
            feedbackSubmitted = true; // Set the flag to true
            feedbackTextarea.disabled = true; // Disable the textarea after submission
            showFeedbackPrompt();
        } else {
            // Show an error message or prevent the submission
            console.log("Error: Please provide feedback before submitting.");
        }
    }
});

function showFeedbackPrompt() {
    feedbackPrompt.style.display = "block";
}
