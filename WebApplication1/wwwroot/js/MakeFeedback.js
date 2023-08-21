// JavaScript for the feedback form and emojis selection
const emojis = document.querySelectorAll(".emoji");
const submitButton = document.getElementById("submitbtn");
const feedbackPrompt = document.querySelector(".feedback-prompt");
const feedbackTextarea = document.querySelector(".feedback-textarea");

let feedbackSubmitted = false; // Flag to track if feedback has been submitted
let selectedEmoji = null; // Variable to store the selected emoji

emojis.forEach((emoji) => {
    emoji.addEventListener("click", () => {
        if (!feedbackSubmitted) {
            const selectedEmojiData = emoji.getAttribute("data-emoji");
            selectedEmoji = selectedEmojiData; // Store the selected emoji

            // Remove selected class from all emojis
            emojis.forEach((emoji) => {
                emoji.classList.remove("selected");
            });

            // Add the selected class to the clicked emoji
            emoji.classList.add("selected");
        }
    });
});

submitButton.addEventListener("click", (event) => {
    event.preventDefault(); // Prevent the default form submission behavior
    if (!feedbackSubmitted) {
        const feedback = feedbackTextarea.value.trim(); // Trim whitespace from the input
        if (feedback !== "") {
            // Check if feedback is not empty
            if (selectedEmoji !== null) {
                // Check if an emoji is selected
                // Perform further processing or send the feedback to the server
                console.log("Feedback submitted:", selectedEmoji, feedback);
                // Display the selected emoji and feedback in the textarea
                feedbackTextarea.value = `${selectedEmoji} ${feedback}`;
                feedbackSubmitted = true; // Set the flag to true
                showFeedbackPrompt();
            } else {
                // Show an error message or prevent the submission
                console.log("Error: Please select an emoji before submitting feedback.");
            }
        }
    }
});

function showFeedbackPrompt() {
    feedbackPrompt.style.display = "block";
    // Hide the prompt after 3 seconds
    setTimeout(() => {
        feedbackPrompt.style.display = "none";
        // Reset the form after hiding the prompt
        feedbackTextarea.value = "";
        emojis.forEach((emoji) => {
            emoji.classList.remove("selected");
        });
        feedbackSubmitted = false;
    }, 3000);
}