# BookSmasher

Currently combines multiple books to produce a new one.

This is done by grabbing every sentence in each book and using a bag of words technique to create feature vectors.
Other features of each sentence include the sentence before it and sentences it's compared against during training.
In addition, I use a KMeans clustering algorithm to assign each sentence a group that is used to help generate the book later.

The user then trains classifiers (currently decision trees, but close to supporting random forests) using some subset of the sentences of the added books
and classifying them manually. Once this is complete, the classifier classifies the remaining sentences, and prints their new order in an
output .txt file based on a score.

The algorithm to generate the final book works as follows:

1. Grab a random sentence from all of the sentences, remove it from available sentences. This will be the first line in the output.

2. Take some subset of the available sentences and predict their scores using the classifier knowing what the previous sentence will be.

3. Choose the sentence with the best score, remove it from the available sentences, add to output in order. Next classification will use this as the
starting sentence.

4. Steps 2 and 3 continue until every sentence is added to the output.


Some exciting avenues that this project could take is supporting image or sound file combinations, and supporting more than just .txt files when combining books.




