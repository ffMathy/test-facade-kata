import { useEffect, useState } from "react";
import type { GetAllBooksResponse } from "../types";

const API_BASE = "http://localhost:5000";

interface Props {
  /** Called when the user clicks a book title to view its details. */
  onSelect: (bookId: number) => void;
}

/**
 * BookList component
 *
 * Fetches all books from /api/books and renders them as a list.
 * Each book shows its title, published year, author name, and genres.
 * Clicking a book title triggers the onSelect callback (used in App.tsx
 * to navigate to the BookDetail component).
 */
export default function BookList({ onSelect }: Props) {
  const [books, setBooks] = useState<GetAllBooksResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch(`${API_BASE}/api/books`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<GetAllBooksResponse[]>;
      })
      .then(setBooks)
      .catch((err: unknown) =>
        setError(err instanceof Error ? err.message : "Unknown error")
      )
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Loading books…</p>;
  if (error) return <p role="alert">Error: {error}</p>;

  return (
    <section>
      <h2>Books</h2>
      {books.length === 0 ? (
        <p>No books found.</p>
      ) : (
        <ul>
          {books.map((book) => {
            // The API now returns genres as a flat array — no nested join table
            const genres = book.genres.map((g) => g.name).join(", ");

            return (
              <li key={book.id}>
                {/* Clicking the title drills into the detail view */}
                <button onClick={() => onSelect(book.id)}>
                  <strong>{book.title}</strong>
                </button>
                {" "}({book.publishedYear})
                {book.author && <span> by {book.author.name}</span>}
                {genres && <span> [{genres}]</span>}
              </li>
            );
          })}
        </ul>
      )}
    </section>
  );
}
