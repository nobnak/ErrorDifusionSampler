# Deterministic Weighted Random Sampling Algorithm for Unity

Error Diffusion is common halftoning algorithm and its sampled dots are determined by input image. So that the sampling position is always same unless the image is changed.

Result may be different on different processors. Because in the current code, accumulation logic is using floating point arithmetics.

## Reuslts

[![Trailer03](http://img.youtube.com/vi/HP3MOO-muBE/mqdefault.jpg)](https://youtu.be/HP3MOO-muBE)
[![Trailer01](http://img.youtube.com/vi/45Z4AgjG8oU/mqdefault.jpg)](https://youtu.be/45Z4AgjG8oU)
[![Trailer01](http://img.youtube.com/vi/ygfhkd1aVBk/mqdefault.jpg)](https://youtu.be/ygfhkd1aVBk)


## Refs
- Error Diffusion : https://en.wikipedia.org/wiki/Error_diffusion
