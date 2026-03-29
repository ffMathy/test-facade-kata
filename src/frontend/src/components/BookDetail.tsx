import { useEffect, useState } from "react";
import type { Book, Review } from "../types";

const API_BASE = "http://localhost:5000";

interface Props {
  bookId: number;
  /** Called when the user wants to go back to the book list. */
  onBack: () => void;
}

/**
 * BookDetail component
 *
 * Fetches a single book (with its author and genres) from /api/books/{id},
 * and its reviews from /api/books/{id}/reviews.
 *
 * This component demonstrates:
 * - Multiple fetch calls for nested resources
 * - Conditional rendering (loading / error / data)
 * - The Author -> Book -> Review relationship in the UI
 */
export default function BookDetail({ bookId, onBack }: Props) {
  const [book, setBook] = useState<Book | null>(null);
  const [reviews, setReviews] = useState<Review[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    // Fetch book detail and reviews in parallel
    Promise.all([
      fetch(`${API_BASE}/api/books/${bookId}`).then((r) => r.json() as Promise<Book>),
      fetch(`${API_BASE}/api/books/${bookId}/reviews`).then((r) => r.json() as Promise<Review[]>),
    ])
      .then(([bookData, reviewData]) => {
        setBook(bookData);
        setReviews(reviewData);
      })
      .catch((err: unknown) =>
        setError(err instanceof Error ? err.message : "Unknown error")
      )
      .finally(() => setLoading(false));
  }, [bookId]);

  if (loading) return <p>Loading book details…</p>;
  if (error) return <p role="alert">Error: {error}</p>;
  if (!book) return <p>Book not found.</p>;

  const genres =
    book.bookGenres?.map((bg) => bg.genre.name).join(", ") ?? "–";

  return (
    <section>
      <button onClick={onBack}>← Back to list</button>
      <h2>{book.title}</h2>
      <p>
        <strong>Published:</strong> {book.publishedYear}
      </p>
      {book.author && (
        <p>
          <strong>Author:</strong> {book.author.name}
        </p>
      )}
      <p>
        <strong>Genres:</strong> {genres}
      </p>

      <h3>Reviews</h3>
      {reviews.length === 0 ? (
        <p>No reviews yet.</p>
      ) : (
        <ul>
          {reviews.map((review) => (
            <li key={review.id}>
              <strong>{review.reviewerName}</strong> — {review.rating}/5
              {review.comment && <p>{review.comment}</p>}
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
