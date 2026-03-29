import { useEffect, useState } from "react";
import type { GetBookByIdResponse } from "../types";

const API_BASE = "http://localhost:5000";

interface Props {
  bookId: number;
  /** Called when the user wants to go back to the book list. */
  onBack: () => void;
}

/**
 * BookDetail component
 *
 * Fetches a single book from /api/books/{id}.
 * The detail response already includes author, genres, and reviews,
 * so only one fetch is needed (no separate /reviews call).
 *
 * This component demonstrates:
 * - Conditional rendering (loading / error / data)
 * - The Author -> Book -> Review relationship in the UI
 */
export default function BookDetail({ bookId, onBack }: Props) {
  const [book, setBook] = useState<GetBookByIdResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch(`${API_BASE}/api/books/${bookId}`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<GetBookByIdResponse>;
      })
      .then(setBook)
      .catch((err: unknown) =>
        setError(err instanceof Error ? err.message : "Unknown error")
      )
      .finally(() => setLoading(false));
  }, [bookId]);

  if (loading) return <p>Loading book details…</p>;
  if (error) return <p role="alert">Error: {error}</p>;
  if (!book) return <p>Book not found.</p>;

  const genres = book.genres.map((g) => g.name).join(", ") || "–";

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
      {book.reviews.length === 0 ? (
        <p>No reviews yet.</p>
      ) : (
        <ul>
          {book.reviews.map((review) => (
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
