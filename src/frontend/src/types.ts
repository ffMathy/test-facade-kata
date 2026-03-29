// ---------------------------------------------------------------------------
// Shared TypeScript types that mirror the backend data models.
// Keeping them in one place makes it easy to update when the API changes.
// ---------------------------------------------------------------------------

export interface Author {
  id: number;
  name: string;
  bio?: string;
}

export interface Genre {
  id: number;
  name: string;
}

/** A book references its author by id and may carry related data. */
export interface Book {
  id: number;
  title: string;
  publishedYear: number;
  authorId: number;
  author?: Author;
  bookGenres?: { genre: Genre }[];
  reviews?: Review[];
}

/** A reader review for a specific book. */
export interface Review {
  id: number;
  rating: number;
  comment?: string;
  reviewerName: string;
  bookId: number;
}
