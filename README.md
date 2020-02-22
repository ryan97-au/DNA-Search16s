# Genomic Sequence Retrieval

## Background
NCBI: “The National Center for Biotechnology Information is part of the United States National Library of Medicine, a branch of the National Institutes of Health. The NCBI is located in Bethesda…” (follow the link (https://www.ncbi.nlm.nih.gov/) to learn more background information)

The task is to retrieve particular DNA repository, commonly referred to as 16S. The Wikipedia article 16S ribosomal RNA describes some of the background information. A collection of curated 16S DNA sequences from NCBI will be used in this assignment. The assignment is concerned with managing access to the collection. The biological science is out of context, but this project is to write a C# program to facilitate command line access to the data file.

A large file (16S.fasta) is available in the Debug file (\Search16s\bin). And the task is to provide a command-line interface that supports search access to the collection. The assignment consists of two deliverable parts. Part I is concerned with simple indexing, searching, and displaying of the information, using exact match searches. Part II is concerned with more complex search tasks, focusing on inexact (partial match) search.

The FASTA file format is simple. There are 20,767 genomic (DNA) sequences in the file. Each sequence is represented by two consecutive lines. The first line begins with the character ‘>’ and contains meta-data describing the source of the DNA sequence, its identity, and other details. The following line contains the respective DNA. For instance, the following 4 lines of text correspond to two sequences; note that the DNA lines are truncated:

`>NR_118941.1 Streptococcus alactolyticus strain NCDO 1091 16S ribosomal RNA, partial sequence` `GAACGGGTGAGTAACGCGTAGGTAACCTGCCTTGTAGCGGGGGATAACTATTGGAAACG ATAGCTAATACCGCATAACAG…`  
`>NR_115365.1 Streptomyces albus strain CSSP327 16S ribosomal RNA, partial sequence` `TCACGGAGAGTTTGATCCTGGCTCAGGACGAACGCTGGCGGCGTGCTTAACACATGCAA GTCGAACGATGAAGC…`

The metadata line always starts with a ‘>’ character. The first sequence-id is NR_118941.1, the name of the organism is Streptococcus alactolyticus, and there is some more descriptive information about the sequence. The next line is the DNA representation, extracted from the 16S region of the bacterial RNA. The second sequence-id is NR_115365.1, the name is Streptomyces albus, and more metadata follows. That line is followed by another line with the DNA representation. Note that the length of the DNA sequence can vary; it is not fixed.

There is one complication with the file format that you must be aware of, and overcome in solving the access requirements. There are 306 sequences in the file where the metadata lists more than one source, but the same DNA. For instance, the following is an example of an identical DNA sequence which was recorded 3 times (from independent sources):

`>NR_074334.1 Archaeoglobus fulgidus DSM 4304 16S ribosomal RNA, complete sequence >NR_118873.1 Archaeoglobus fulgidus DSM 4304 strain VC-16 16S ribosomal RNA, complete sequence >NR_119237.1 Archaeoglobus fulgidus DSM 4304 strain VC-16 16S ribosomal RNA, complete sequence`  
`ATTCTGGTTGATCCTGCCAGAGGCCGCTGCTATCCGGCTGGGACTAAGCCATGCGAGTCAAGGGGCTTGTATCCCTTCGGGGATGCAAGCACCGGCGGACGGCTCAGTAACACGTGGACAACCTGCCCTCGGGTGGGGGATAACCCCGGGAAACTGGGGCTAATCCCCCATAGG…`

When considering the solutions to the access requirements, bear this in mind, lest the search miss some sequence metadata.

## Requirements
An important mandatory requirement is that the 16S.fasta file must not be loaded into memory – the search is disk based. This is because in the intended applications the file size may be too large to fit in memory, or some other resource constraints prevent the application from having access to enough memory –e.g. in a shared environment.

#### 1. Sequential access using a starting position in the file.
         Example: Search16s -level1 16S.fasta 273 1
         
         Program output:
         >NR_025900.1 Thermus aquaticus strain YT-1 16S ribosomal RNA, partial sequence
         GCTCAGGGTGAACGCTGGCGGCGTGCCTAAGACATGCAAGTCGTGCGGGCCGTGG GGTATCTCACGGTCAGCGGCGGACGGGTGAGTAACGCGTGGGTGACCTACCCGGA
         AGAGGGGGACAACATGGGGAAACCCAGGCTAATCCCCCATGTGGACACAT…
##### Save to file
         Example: Search16s -level1 16S.fasta 273 N > myfile.fasta
         
         The program will capture the output in a file called myfile.fasta in the current directory. N is the number of sequences to
         output (not the number of lines, which is 2N)

#### 2. Sequential access to a specific sequence by sequence-id.
         Example: Search16s -level2 16S.fasta NR_115365.1
         
         Program output:
         >NR_115365.1 Streptomyces albus strain CSSP327 16S ribosomal RNA, partial sequence
         TCACGGAGAGTTTGATCCTGGCTCAGGACGAACGCTGGCGGCGTGCTTAACACATG CAAGTCGAACGATGAAGCCCTTCG…

#### 3. Sequential access to find a set of sequence-ids given in a query file, and writing the output to a specified result file.
         Example: Search16s -level3 16S.fasta query.txt results.txt
         
         Suppose that the input file query.txt contains 3 lines:
         NR_115365.1
         NR_999999.9
         NR_118941.1
Further suppose that the second sequence-id cannot be found. The output file, `results.txt`, will contain ONLY the sequences that were found, first and third in the list above, with the same format as with previous levels.

#### 4. Indexed file access, implementing direct access to sequences.
##### 1) Before running any searches, the fasta file must be indexed.
         Example: IndexSequence16s 16S.fasta 16S.index 
Two file names are specified, the fasta file name and the index file name (these are run-time variables, not fixed hard-code file names). The program creates a sequenceid index to the fasta file. The index supports direct access to sequences, by sequenceid. Specifically, the program creates an index file. Each line consists of a `sequenceid` and `file-offset`. `Note that the index files indexes sequences, not lines`.  
###### The indexing program, IndexSequence16s, should be implemented as a separate Visual studio project.
         An example index file:
         NR_115365.1 0
         NR_999999.9 531
         …
         NR_118941.1 1236733
         …
The first sequence above if found on a line with byte offset 0 from the beginning of the file, the second sequence can be found at offset 531 from the beginning of the file, and the next sequence above (somewhere, later in the file) can be found at offset 1236733 bytes.
##### 2) The index file is created once only, and afterwards the file can be searched many times using the index file to facilitate direct access to the sequences file.
         Example: Search16s -level4 16S.fasta 16S.index query.txt results.txt
The program operates in the same manner as the level 3 program, but instead of using a sequential file scan, it uses the index, specified with the additional index filename. When the program starts it loads the index file into memory. The index file is much smaller of course, and has assumed that it fits in memory. The program then uses it to search for query sequences-ids. The sequences are then read from the fasta file using `direct access` rather than a sequential scan by using the file offsets from the index, and the `Seek()` method to go directly to the sequences on disk.

#### 5. Search for an exact match of a DNA query string.
         Example: Search16s -level5 16S.fasta CTGGTACGGTCAACTTGCTCTAAG
         
         For instance:
         Program output:
         NR_115365.1
         NR_123456.1
         NR_118941.1
         NR_432567.1
         NR_118900.1

#### 6. Search for a sequence meta-data containing a given word.
         Example: Search16s -level6 16S.fasta Streptomyces
         
         For instance:
         Program output:
         NR_026530.1
         NR_026529.1
         NR_119348.1
These sequence-ids correspond to sequences that match the word Streptomyces:  
`>NR_026530.1 Streptomyces macrosporus strain A1201 16S ribosomal RNA, partial sequence`   `GACGAACGCTGGCGGCGTGCTTAACACATGCAAGTCGAACGATGAACCTCCTTCGGGAG GGGATTAGTGGCGAACGGGTG…`  
`>NR_026529.1 Streptomyces thermolineatus strain A1484 16S ribosomal RNA, partial sequence`   `GACGAACGCTGGCGGCGTGCTTAACACATGCAAGTCGAACGGTGAAGCCCTTCGGGGTG GATCAGTGGCGAACGGGTGAG…`  
`>NR_119348.1 Streptomyces gougerotii strain DSM 40324 16S ribosomal RNA, partial sequence`   `AACGCTGGCGGCGTGCTTAACACATGCAAGTCGAACGATGAAGCCCTTCGGGGTGGATT AGTGGCGAACGGGTGAGTAAC…`

#### 7. Search for a sequence containing wild cards.
         A “*” stands for any number of characters in the same position. Display all matching sequences to the screen
         Example: Search16s -level7 16S.fasta ACTG*GTAC*CA
         
         This should match, for instance:
         ACTGGTACCA
         ACTGCGTACCA
         ACTGGTACGCA
         ACTGAGTACTCA
         ACTGACGTACTGTGCCA
         ACTGACCGTACTGCA
         ACTGGTACTGTCA
         …
         
         Hint: Use a regular expression.
